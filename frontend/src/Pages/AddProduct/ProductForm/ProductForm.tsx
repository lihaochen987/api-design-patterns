import { Form, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { components } from '../../../Shared/types';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { fetchClient } from '../../../Shared/fetch-client.ts';
import {
  ButtonGroup,
  ErrorBanner,
  ErrorMessage,
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

type CreateProductRequest = components['schemas']['CreateProductRequest'];
type CreateProductResponse = components['schemas']['CreateProductResponse'];

const productSchema = z.object({
  name: z.string().min(1, 'Product name is required'),
  pricing: z.object({
    basePrice: z.number().positive('Base price must be positive'),
    discountPercentage: z.number().positive('Discount Percentage must be positive'),
    taxRate: z.number().positive('Tax rate must be positive'),
  }),
  category: z.enum(
    [
      'petfood',
      'toys',
      'collarsAndLeashes',
      'groomingAndHygiene',
      'beds',
      'feeders',
      'travelAccessories',
      'clothing',
    ],
    {
      errorMap: () => ({ message: 'Please select a valid category' }),
    }
  ),
  dimensions: z.object({
    width: z.number().positive('Width must be positive'),
    height: z.number().positive('Height must be positive'),
    length: z.number().positive('Length must be positive'),
  }),
});

type ProductFormData = z.infer<typeof productSchema>;

const useCreateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (newProduct: CreateProductRequest): Promise<CreateProductResponse> => {
      console.log('Creating product:', newProduct);

      const { data, error } = await fetchClient.POST('/product', {
        body: newProduct,
      });

      if (error) {
        console.error('API error:', error);
        throw new Error(String(error.message) || 'Failed to create product');
      }

      if (!data) {
        throw new Error('No data returned from the server');
      }

      return data;
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: ['products'] });
    },
  });
};

const AddProductPage = () => {
  const navigate = useNavigate();
  const createProduct = useCreateProduct();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
  });

  const onSubmit = async (data: ProductFormData) => {
    try {
      const apiData: CreateProductRequest = {
        ...data,
      };
      await createProduct.mutateAsync(apiData);
      console.log('Product created successfully');
      navigate('/');
    } catch (err) {
      console.error('Error creating product:', err);
      alert('Failed to create product. Please try again.');
    }
  };

  const handleCancel = () => {
    navigate('/');
  };

  return (
    <PageContainer>
      <PageTitle>Add New Product</PageTitle>

      <Form onSubmit={handleSubmit(onSubmit)}>
        {createProduct.isPending && <LoadingMessage>Creating product...</LoadingMessage>}
        {createProduct.isError && <ErrorBanner>Error: {createProduct.error.message}</ErrorBanner>}

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
            max="100"
            step="0.01"
            {...register('pricing.taxRate', { valueAsNumber: true })}
          />
          {errors.pricing?.taxRate && <ErrorMessage>{errors.pricing.taxRate.message}</ErrorMessage>}
        </FormGroup>

        <FormGroup>
          <Label htmlFor="discountPercentage">Discount Percentage (%)</Label>
          <Input
            type="number"
            id="discountPercentage"
            max="100"
            step="0.01"
            {...register('pricing.discountPercentage', { valueAsNumber: true })}
          />
          {errors.pricing?.taxRate && <ErrorMessage>{errors.pricing.taxRate.message}</ErrorMessage>}
        </FormGroup>

        <FormGroup>
          <Label htmlFor="width">Product Width</Label>
          <Input
            type="number"
            id="width"
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
            {...register('dimensions.length', { valueAsNumber: true })}
          />
          {errors.dimensions?.length && (
            <ErrorMessage>{errors.dimensions.length.message}</ErrorMessage>
          )}
        </FormGroup>

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

        <ButtonGroup>
          <SecondaryButton type="button" onClick={handleCancel}>
            Cancel
          </SecondaryButton>
          <PrimaryButton type="submit">Add Product</PrimaryButton>
        </ButtonGroup>
      </Form>
    </PageContainer>
  );
};

export default AddProductPage;
