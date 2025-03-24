import { useNavigate } from 'react-router-dom';
import { FormProvider, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  ButtonGroup,
  ErrorBanner,
  ErrorMessage,
  Form,
  FormGroup,
  Input,
  Label,
  LoadingMessage,
  PageContainer,
  PageTitle,
  PrimaryButton,
  SecondaryButton,
  Select,
} from './ProductForm.styles.ts';
import {
  CreateProductRequest,
  ProductFormData,
  productSchema,
  petFoodSchema,
  groomingAndHygieneSchema,
  otherProductSchema,
} from './ProductForm.types.ts';
import { useCreateProduct } from './ProductForm.hooks.ts';
import { PetFoodForm } from './PetFoodProductForm/PetFoodProductForm.tsx';
import { GroomingAndHygieneForm } from './GroomingAndHygieneForm/GroomingAndHygieneForm.tsx';
import { z } from 'zod';

const AddProductPage = () => {
  const navigate = useNavigate();
  const createProduct = useCreateProduct();

  // Create a specialized form based on the selected category
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
    // This is important - it ensures all fields are included in the form data
    shouldUnregister: false,
  });

  const {
    handleSubmit,
    register,
    watch,
    setValue,
    formState: { errors },
  } = methods;

  const selectedCategory = watch('category');

  const onSubmit = async (formData: ProductFormData) => {
    try {
      console.log('Raw form data:', formData);

      // Create a properly typed API request based on the category
      let apiData: CreateProductRequest;

      switch (formData.category) {
        case 'petFood': {
          // We need to ensure we're working with the complete pet food schema
          // First, validate it against the petFoodSchema
          const result = petFoodSchema.safeParse(formData);

          if (!result.success) {
            console.error('PetFood validation failed:', result.error);
            throw new Error(`Validation failed: ${result.error.message}`);
          }

          // Use the validated data which should have all the required fields
          const petFoodData = result.data;
          console.log('Validated pet food data:', petFoodData);

          // Create the API request with all the required fields
          apiData = {
            category: petFoodData.category,
            name: petFoodData.name,
            pricing: petFoodData.pricing,
            dimensions: petFoodData.dimensions,
            // Explicitly include all pet food specific fields
            ageGroup: petFoodData.ageGroup,
            breedSize: petFoodData.breedSize,
            ingredients: petFoodData.ingredients,
            storageInstructions: petFoodData.storageInstructions,
            weightKg: petFoodData.weightKg,
          };
          break;
        }

        case 'groomingAndHygiene': {
          // Similar process for grooming and hygiene products
          const result = groomingAndHygieneSchema.safeParse(formData);
          if (!result.success) {
            throw new Error(`Validation failed: ${result.error.message}`);
          }
          apiData = result.data;
          break;
        }

        default: {
          // For other product types
          const result = otherProductSchema.safeParse(formData);
          if (!result.success) {
            throw new Error(`Validation failed: ${result.error.message}`);
          }
          apiData = result.data;
          break;
        }
      }

      console.log('Final API data being sent:', apiData);
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
    <PageContainer>
      <PageTitle>Add New Product</PageTitle>

      <FormProvider {...methods}>
        <Form id={'add-product-form'} onSubmit={handleSubmit(onSubmit)}>
          {createProduct.isPending && <LoadingMessage>Creating product...</LoadingMessage>}
          {createProduct.isError && <ErrorBanner>Error: {createProduct.error.message}</ErrorBanner>}

          <FormGroup>
            <Label htmlFor="category">Category</Label>
            <Select id="category" {...register('category')}>
              <option value="">Select a category</option>
              <option value="petFood">Pet Food</option>
              <option value="toys">Toys</option>
              <option value="collarsAndLeashes">Collars and Leashes</option>
              <option value="groomingAndHygiene">Grooming and Hygiene</option>
              <option value="beds">Beds</option>
              <option value="feeders">Feeders</option>
              <option value="travelAccessories">Travel Accessories</option>
              <option value="clothing">Clothing</option>
            </Select>
            {errors.category && <ErrorMessage>{errors.category.message}</ErrorMessage>}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="name">Product Name</Label>
            <Input id="name" {...register('name')} />
            {errors.name && <ErrorMessage>{errors.name.message}</ErrorMessage>}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="basePrice">Base Price ($)</Label>
            <Input
              type="number"
              id="basePrice"
              min="0.01"
              step="0.01"
              {...register('pricing.basePrice', { valueAsNumber: true })}
            />
            {errors.pricing?.basePrice && (
              <ErrorMessage>{errors.pricing.basePrice.message}</ErrorMessage>
            )}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="taxRate">Tax Rate (%)</Label>
            <Input
              type="number"
              id="taxRate"
              min="0"
              max="100"
              step="0.1"
              {...register('pricing.taxRate', { valueAsNumber: true })}
            />
            {errors.pricing?.taxRate && (
              <ErrorMessage>{errors.pricing.taxRate.message}</ErrorMessage>
            )}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="discountPercentage">Discount Percentage (%)</Label>
            <Input
              type="number"
              id="discountPercentage"
              min="0"
              max="100"
              step="0.1"
              {...register('pricing.discountPercentage', { valueAsNumber: true })}
            />
            {errors.pricing?.discountPercentage && (
              <ErrorMessage>{errors.pricing.discountPercentage.message}</ErrorMessage>
            )}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="width">Product Width</Label>
            <Input
              type="number"
              id="width"
              min="0"
              step="0.1"
              {...register('dimensions.width', { valueAsNumber: true })}
            />
            {errors.dimensions?.width && (
              <ErrorMessage>{errors.dimensions.width.message}</ErrorMessage>
            )}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="height">Product Height</Label>
            <Input
              type="number"
              id="height"
              min="0"
              step="0.1"
              {...register('dimensions.height', { valueAsNumber: true })}
            />
            {errors.dimensions?.height && (
              <ErrorMessage>{errors.dimensions.height.message}</ErrorMessage>
            )}
          </FormGroup>

          <FormGroup>
            <Label htmlFor="length">Product Length</Label>
            <Input
              type="number"
              id="length"
              min="0"
              step="0.1"
              {...register('dimensions.length', { valueAsNumber: true })}
            />
            {errors.dimensions?.length && (
              <ErrorMessage>{errors.dimensions.length.message}</ErrorMessage>
            )}
          </FormGroup>

          {selectedCategory && renderCategoryFields()}

          <ButtonGroup>
            <SecondaryButton type="button" onClick={handleCancel}>
              Cancel
            </SecondaryButton>
            <PrimaryButton id={'add-product-form'} type="submit">
              Add Product
            </PrimaryButton>
          </ButtonGroup>
        </Form>
      </FormProvider>
    </PageContainer>
  );
};

export default AddProductPage;
