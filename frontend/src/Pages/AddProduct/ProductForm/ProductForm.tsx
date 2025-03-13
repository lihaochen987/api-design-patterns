import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { components } from '../../../../types';

type CreateProductRequest = components['schemas']['CreateProductRequest'];

const productSchema = z.object({
  name: z.string().min(1, 'Product name is required'),
  pricing: z.object({
    basePrice: z.number().positive('Base price must be positive'),
    taxRate: z.number().positive('Tax rate must be positive'),
  }),
  dimensions: z.object({
    width: z.number().positive('Width must be positive'),
    height: z.number().positive('Height must be positive'),
    length: z.number().positive('Length must be positive'),
  }),
  category: z.enum(['food', 'toys', 'accessories', 'health', 'grooming'], {
    errorMap: () => ({ message: 'Please select a valid category' }),
  }),
});

type ProductFormData = z.infer<typeof productSchema>;

const AddProductPage = () => {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
    defaultValues: {
      name: '',
      pricing: {
        basePrice: undefined,
        taxRate: undefined,
      },
      dimensions: {
        width: undefined,
        height: undefined,
        length: undefined,
      },
      category: undefined,
    },
  });

  const onSubmit = async (data: ProductFormData) => {
    try {
      const apiData: CreateProductRequest = {
        ...data,
      };

      console.log('Submitting product:', apiData);

      alert('Product added successfully!');
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
          <Label htmlFor="taxRate">Tax Rate ($)</Label>
          <Input
            type="number"
            id="taxRate"
            min="0.01"
            step="0.01"
            {...register('pricing.taxRate', { valueAsNumber: true })}
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
            <option value="food">Pet Food</option>
            <option value="toys">Toys</option>
            <option value="accessories">Accessories</option>
            <option value="health">Health & Wellness</option>
            <option value="grooming">Grooming</option>
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

// Styled components remain the same
const PageContainer = styled.div`
  max-width: 800px;
  margin: 100px auto 50px;
  padding: 0 20px;
`;

const PageTitle = styled.h1`
  font-size: 2rem;
  color: #333;
  margin-bottom: 30px;
`;

const Form = styled.form`
  background-color: #fff;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
`;

const FormGroup = styled.div`
  margin-bottom: 20px;
`;

const Label = styled.label`
  display: block;
  font-weight: 500;
  margin-bottom: 8px;
  color: #333;
`;

const Input = styled.input`
  width: 100%;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;

  &:focus {
    border-color: #4a90e2;
    outline: none;
  }
`;

const Select = styled.select`
  width: 100%;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;

  &:focus {
    border-color: #4a90e2;
    outline: none;
  }
`;

const ButtonGroup = styled.div`
  display: flex;
  justify-content: flex-end;
  gap: 15px;
  margin-top: 30px;
`;

const Button = styled.button`
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s;
`;

const PrimaryButton = styled(Button)`
  background-color: #4a90e2;
  color: white;

  &:hover {
    background-color: #3a80d2;
  }
`;

const SecondaryButton = styled(Button)`
  background-color: #f5f5f5;
  color: #333;

  &:hover {
    background-color: #e5e5e5;
  }
`;

const ErrorMessage = styled.p`
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.5rem;
`;
