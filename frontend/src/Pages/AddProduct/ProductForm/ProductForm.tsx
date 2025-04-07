import { useNavigate } from 'react-router-dom';
import { FormProvider, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  CreateProductRequest,
  ProductFormData,
  productSchema,
  petFoodSchema,
  groomingAndHygieneSchema,
  otherProductSchema,
} from './ProductForm.types.ts';
import { useCreateProduct } from './ProductForm.hooks.ts';
import { PetFoodForm } from './SubForms/PetFoodProductForm.tsx';
import { GroomingAndHygieneForm } from './SubForms/GroomingAndHygieneForm.tsx';

const AddProductPage = () => {
  const navigate = useNavigate();
  const createProduct = useCreateProduct();

  const methods = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
    defaultValues: {
      category: undefined,
      pricing: {
        basePrice: undefined,
        discountPercentage: undefined,
        taxRate: undefined,
      },
      dimensions: {
        width: undefined,
        height: undefined,
        length: undefined,
      },
    },
    shouldUnregister: false,
  });

  const {
    handleSubmit,
    register,
    watch,
    formState: { errors },
  } = methods;

  const selectedCategory = watch('category');

  const onSubmit = async (formData: ProductFormData) => {
    try {
      let apiData: CreateProductRequest;

      switch (formData.category) {
        case 'petFood': {
          const result = petFoodSchema.safeParse(formData);

          if (!result.success) {
            console.error('PetFood validation failed:', result.error);
            throw new Error(`Validation failed: ${result.error.message}`);
          }

          const petFoodData = result.data;
          console.log('Validated pet food data:', petFoodData);

          apiData = {
            category: petFoodData.category,
            name: petFoodData.name,
            pricing: petFoodData.pricing,
            dimensions: petFoodData.dimensions,
            ageGroup: petFoodData.ageGroup,
            breedSize: petFoodData.breedSize,
            ingredients: petFoodData.ingredients,
            storageInstructions: petFoodData.storageInstructions,
            weightKg: petFoodData.weightKg,
          };
          break;
        }

        case 'groomingAndHygiene': {
          const result = groomingAndHygieneSchema.safeParse(formData);
          if (!result.success) {
            throw new Error(`Validation failed: ${result.error.message}`);
          }
          apiData = result.data;
          break;
        }

        default: {
          const result = otherProductSchema.safeParse(formData);
          if (!result.success) {
            throw new Error(`Validation failed: ${result.error.message}`);
          }
          apiData = result.data;
          break;
        }
      }

      await createProduct.mutateAsync(apiData);
      navigate('/');
    } catch (err) {
      console.error('Error creating product:', err);
      alert('Failed to create product. Please try again.');
    }
  };

  const handleCancel = () => {
    navigate('/');
  };

  const renderCategoryFields = () => {
    switch (selectedCategory) {
      case 'petFood':
        return <PetFoodForm />;

      case 'groomingAndHygiene':
        return <GroomingAndHygieneForm />;

      default:
        return null;
    }
  };

  return (
    <div className={'l-page-container'}>
      <h1 className={'page-title'}>Add New Product</h1>

      <FormProvider {...methods}>
        <form className={'l-form'} id={'add-product-form'} onSubmit={handleSubmit(onSubmit)}>
          {createProduct.isPending && (
            <div className={'form-loading-message'}>Creating product...</div>
          )}
          {createProduct.isError && (
            <div className={'form-error-banner'}>Error: {createProduct.error.message}</div>
          )}

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="category">
              Category
            </label>
            <select className={'form-select'} id="category" {...register('category')}>
              <option value="">Select a category</option>
              <option value="petFood">Pet Food</option>
              <option value="toys">Toys</option>
              <option value="collarsAndLeashes">Collars and Leashes</option>
              <option value="groomingAndHygiene">Grooming and Hygiene</option>
              <option value="beds">Beds</option>
              <option value="feeders">Feeders</option>
              <option value="travelAccessories">Travel Accessories</option>
              <option value="clothing">Clothing</option>
            </select>
            {errors.category && <p className={'form-error-message'}>hello</p>}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="name">
              Product Name
            </label>
            <input className={'form-input'} id="name" {...register('name')} />
            {errors.name && <p className={'form-error-message'}>{errors.name.message}</p>}
          </div>

          <div className="l-constrained">
            <label className={'form-label'} htmlFor="basePrice">
              Base Price ($)
            </label>
            <input
              className={'form-input'}
              type="number"
              id="basePrice"
              min="0.01"
              step="0.01"
              {...register('pricing.basePrice', { valueAsNumber: true })}
            />
            {errors.pricing?.basePrice && (
              <p className={'form-error-message'}>{errors.pricing.basePrice.message}</p>
            )}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="taxRate">
              Tax Rate (%)
            </label>
            <input
              className={'form-input'}
              type="number"
              id="taxRate"
              min="0"
              max="100"
              step="0.1"
              {...register('pricing.taxRate', { valueAsNumber: true })}
            />
            {errors.pricing?.taxRate && (
              <p className={'form-error-message'}>{errors.pricing.taxRate.message}</p>
            )}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="discountPercentage">
              Discount Percentage (%)
            </label>
            <input
              className={'form-input'}
              type="number"
              id="discountPercentage"
              min="0"
              max="100"
              step="0.1"
              {...register('pricing.discountPercentage', { valueAsNumber: true })}
            />
            {errors.pricing?.discountPercentage && (
              <p className={'form-error-message'}>{errors.pricing.discountPercentage.message}</p>
            )}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="width">
              Product Width
            </label>
            <input
              className={'form-input'}
              type="number"
              id="width"
              min="0"
              step="0.1"
              {...register('dimensions.width', { valueAsNumber: true })}
            />
            {errors.dimensions?.width && (
              <p className={'form-error-message'}>{errors.dimensions.width.message}</p>
            )}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="height">
              Product Height
            </label>
            <input
              className={'form-input'}
              type="number"
              id="height"
              min="0"
              step="0.1"
              {...register('dimensions.height', { valueAsNumber: true })}
            />
            {errors.dimensions?.height && (
              <p className={'form-error-message'}>{errors.dimensions.height.message}</p>
            )}
          </div>

          <div className={'l-constrained'}>
            <label className={'form-label'} htmlFor="length">
              Product Length
            </label>
            <input
              className={'form-input'}
              type="number"
              id="length"
              min="0"
              step="0.1"
              {...register('dimensions.length', { valueAsNumber: true })}
            />
            {errors.dimensions?.length && (
              <p className={'form-error-message'}>{errors.dimensions.length.message}</p>
            )}
          </div>

          {selectedCategory && renderCategoryFields()}

          <div className={'l-button-group'}>
            <button className={'form-button-primary'} type="button" onClick={handleCancel}>
              Cancel
            </button>
            <button className={'form-button-secondary'} id={'add-product-form'} type="submit">
              Add Product
            </button>
          </div>
        </form>
      </FormProvider>
    </div>
  );
};

export default AddProductPage;
