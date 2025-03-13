import { components } from '../../../Shared/types';
import { $api } from '../../../Shared/fetch-client.ts';
import { PetFoodCard } from './Cards/PetFoodCard/PetFoodCard.tsx';
import { GroomingAndHygieneCard } from './Cards/GroomingAndHygieneCard/GroomingAndHygieneCard.tsx';
import { DefaultProductCard } from './Cards/DefaultProductCard/DefaultProductCard.tsx';
import styled from 'styled-components';

export const ProductList = () => {
  const { data, isLoading } = $api.useQuery('get', '/products');

  if (isLoading) {
    return <div>Loading...</div>;
  }

  const renderProduct = (
    product:
      | components['schemas']['GetProductResponse']
      | components['schemas']['GetPetFoodResponse']
      | components['schemas']['GetGroomingAndHygieneResponse']
  ) => {
    switch (product.category) {
      case 'PetFood':
        return <PetFoodCard product={product as components['schemas']['GetPetFoodResponse']} />;
      case 'GroomingAndHygiene':
        return (
          <GroomingAndHygieneCard
            product={product as components['schemas']['GetGroomingAndHygieneResponse']}
          />
        );
      default:
        return <DefaultProductCard product={product} />;
    }
  };

  return (
    <ProductListContainer>
      {data?.results?.map(product => (
        <div key={product.id} className="product-item">
          {renderProduct(product)}
        </div>
      ))}
    </ProductListContainer>
  );
};

const ProductListContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
  padding: 1.25rem;
`;
