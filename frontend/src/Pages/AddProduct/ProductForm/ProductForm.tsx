import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';

interface ProductFormData {
  name: string;
  price: number;
  description: string;
  category: string;
  imageUrl: string;
  stockQuantity: number;
}

const AddProductPage: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<ProductFormData>({
    name: '',
    price: 0,
    description: '',
    category: '',
    imageUrl: '',
    stockQuantity: 0,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'price' || name === 'stockQuantity' ? parseFloat(value) : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Here you would typically send the data to your API
    console.log('Submitting product:', formData);

    // Simulate successful product creation
    alert('Product added successfully!');
    navigate('/products');
  };

  const handleCancel = () => {
    navigate('/products');
  };

  return (
    <PageContainer>
      <PageTitle>Add New Product</PageTitle>

      <Form onSubmit={handleSubmit}>
        <FormGroup>
          <Label htmlFor="name">Product Name</Label>
          <Input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
          />
        </FormGroup>

        <FormGroup>
          <Label htmlFor="price">Price ($)</Label>
          <Input
            type="number"
            id="price"
            name="price"
            min="0.01"
            step="0.01"
            value={formData.price || ''}
            onChange={handleChange}
            required
          />
        </FormGroup>

        <FormGroup>
          <Label htmlFor="description">Description</Label>
          <TextArea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            required
          />
        </FormGroup>

        <FormGroup>
          <Label htmlFor="category">Category</Label>
          <Select
            id="category"
            name="category"
            value={formData.category}
            onChange={handleChange}
            required
          >
            <option value="">Select a category</option>
            <option value="food">Pet Food</option>
            <option value="toys">Toys</option>
            <option value="accessories">Accessories</option>
            <option value="health">Health & Wellness</option>
            <option value="grooming">Grooming</option>
          </Select>
        </FormGroup>

        <FormGroup>
          <Label htmlFor="imageUrl">Image URL</Label>
          <Input
            type="url"
            id="imageUrl"
            name="imageUrl"
            value={formData.imageUrl}
            onChange={handleChange}
            required
          />
        </FormGroup>

        <FormGroup>
          <Label htmlFor="stockQuantity">Stock Quantity</Label>
          <Input
            type="number"
            id="stockQuantity"
            name="stockQuantity"
            min="0"
            step="1"
            value={formData.stockQuantity || ''}
            onChange={handleChange}
            required
          />
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

const TextArea = styled.textarea`
  width: 100%;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  min-height: 120px;
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
