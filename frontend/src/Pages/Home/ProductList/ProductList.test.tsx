import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ProductList } from './ProductList.tsx';
import { $api } from '../../../Shared/fetch-client.ts';

jest.mock('../../../Shared/fetch-client.ts', () => ({
  $api: {
    useQuery: jest.fn(),
  },
}));

const renderProductList = () => {
  return render(<ProductList />);
};

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

    renderProductList();

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  test('renders products with correct card components based on category', () => {
    const mockProducts = {
      results: [
        {
          $type: 'PetFood',
          ageGroup: 'Puppy',
          breedSize: 'Small',
          ingredients: 'Chicken, Rice, Vitamins',
          nutritionalInfo: {},
          storageInstructions: 'Keep in a cool, dry place',
          weightKg: '5.0',
          id: '1',
          name: 'Dry Dog Food',
          price: '51.06',
          category: 'PetFood',
          dimensions: {
            length: '10.0',
            width: '5.0',
            height: '3.0',
          },
        },
        {
          id: '2',
          name: 'Chew Toy',
          price: '14.55',
          category: 'Toys',
          dimensions: {
            length: '6.0',
            width: '6.0',
            height: '4.0',
          },
        },
        {
          $type: 'GroomingAndHygiene',
          isNatural: true,
          isHypoAllergenic: true,
          usageInstructions: 'Apply a small amount to wet coat, lather, and rinse thoroughly.',
          isCrueltyFree: true,
          safetyWarnings: 'Avoid contact with eyes.',
          id: '8',
          name: 'Dog Shampoo',
          price: '10.50',
          category: 'GroomingAndHygiene',
          dimensions: {
            length: '8.0',
            width: '4.0',
            height: '2.0',
          },
        },
      ],
    };

    ($api.useQuery as jest.Mock).mockReturnValue({
      data: mockProducts,
      isLoading: false,
    });

    renderProductList();

    expect(screen.getByText(/Dry Dog Food/i)).toBeInTheDocument();
    expect(screen.getByText(/Dog Shampoo/i)).toBeInTheDocument();
    expect(screen.getByText(/Chew Toy/i)).toBeInTheDocument();

    expect(screen.getByText(/Puppy/i)).toBeInTheDocument();
    expect(screen.getByText(/Small/i)).toBeInTheDocument();
    expect(screen.getByText(/Chicken, Rice, Vitamins/i)).toBeInTheDocument();
    expect(screen.getByText(/Keep in a cool, dry place/i)).toBeInTheDocument();
    expect(screen.getByText(/5.0 kg/i)).toBeInTheDocument();

    // expect(screen.getByText(/Natural/i)).toBeInTheDocument();
    // expect(screen.getByText(/Hypoallergenic/i)).toBeInTheDocument();
    // expect(screen.getByText(/Cruelty Free/i)).toBeInTheDocument();
    // expect(
    //   screen.getByText(/Apply a small amount to wet coat, lather, and rinse thoroughly./i)
    // ).toBeInTheDocument();
    // expect(screen.getByText(/Avoid contact with eyes./i)).toBeInTheDocument();

    // const toyCard = screen
    //   .getByText(/Chew Toy/i)
    //   .closest('[data-testid="product-card"]')! as HTMLElement;
    // expect(within(toyCard).getByText(/Dimensions:/i)).toBeInTheDocument();
    // expect(within(toyCard).getByText(/6.0 x 6.0 x 4.0/i)).toBeInTheDocument();
  });

  test('calls useQuery with correct parameters', () => {
    ($api.useQuery as jest.Mock).mockReturnValue({
      data: { results: [] },
      isLoading: false,
    });

    renderProductList();

    // Verify the API call parameters
    expect($api.useQuery).toHaveBeenCalledWith('get', '/products');
  });
});
