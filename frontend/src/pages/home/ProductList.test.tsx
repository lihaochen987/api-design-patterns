import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ProductList } from './ProductList';
import { $api } from '../../shared/fetch-client';
import { PetFoodCard } from './cards/PetFoodCard';
import { GroomingAndHygieneCard } from './cards/GroomingAndHygieneCard';
import { DefaultProductCard } from './cards/DefaultProductCard';

// Mock the API client
jest.mock('../../shared/fetch-client', () => ({
  $api: {
    useQuery: jest.fn(),
  },
}));

// Mock the card components
jest.mock('./cards/PetFoodCard', () => ({
  PetFoodCard: jest.fn(() => <div data-testid="pet-food-card">Pet Food Card</div>),
}));

jest.mock('./cards/GroomingAndHygieneCard', () => ({
  GroomingAndHygieneCard: jest.fn(() => (
    <div data-testid="grooming-hygiene-card">Grooming Card</div>
  )),
}));

jest.mock('./cards/DefaultProductCard', () => ({
  DefaultProductCard: jest.fn(() => <div data-testid="default-product-card">Default Card</div>),
}));

describe('ProductList Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('renders loading state when data is being fetched', () => {
    // Mock loading state
    ($api.useQuery as jest.Mock).mockReturnValue({
      data: null,
      isLoading: true,
    });

    render(<ProductList />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('renders empty state when no results are returned', () => {
    // Mock empty data
    ($api.useQuery as jest.Mock).mockReturnValue({
      data: { results: [] },
      isLoading: false,
    });

    render(<ProductList />);

    // Check that container is rendered but empty
    const container = document.querySelector('.product-item');
    expect(container).not.toBeInTheDocument();
  });

  test('renders correct card components based on product categories', () => {
    // Mock API response with different product types
    const mockProducts = {
      results: [
        { id: '1', category: 'PetFood', name: 'Dog Food' },
        { id: '2', category: 'GroomingAndHygiene', name: 'Dog Shampoo' },
        { id: '3', category: 'Toys', name: 'Ball' },
      ],
    };

    ($api.useQuery as jest.Mock).mockReturnValue({
      data: mockProducts,
      isLoading: false,
    });

    render(<ProductList />);

    // Check if the correct number of product items are rendered
    const productItems = document.querySelectorAll('.product-item');
    expect(productItems.length).toBe(3);

    // Check if each card type is rendered correctly
    expect(screen.getByTestId('pet-food-card')).toBeInTheDocument();
    expect(screen.getByTestId('grooming-hygiene-card')).toBeInTheDocument();
    expect(screen.getByTestId('default-product-card')).toBeInTheDocument();

    // Verify each card component was called with the correct props
    expect(PetFoodCard).toHaveBeenCalledWith(
      { product: mockProducts.results[0] },
      expect.anything()
    );

    expect(GroomingAndHygieneCard).toHaveBeenCalledWith(
      { product: mockProducts.results[1] },
      expect.anything()
    );

    expect(DefaultProductCard).toHaveBeenCalledWith(
      { product: mockProducts.results[2] },
      expect.anything()
    );
  });

  test('calls useQuery with correct parameters', () => {
    ($api.useQuery as jest.Mock).mockReturnValue({
      data: { results: [] },
      isLoading: false,
    });

    render(<ProductList />);

    // Verify the API call parameters
    expect($api.useQuery).toHaveBeenCalledWith('get', '/products');
  });
});
