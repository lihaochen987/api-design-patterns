import React, { useState } from 'react';
import { components } from '../../../Shared/types';
import { $api } from '../../../Shared/fetch-client.ts';
import { PetFoodCard } from './Cards/PetFoodCard/PetFoodCard.tsx';
import { GroomingAndHygieneCard } from './Cards/GroomingAndHygieneCard/GroomingAndHygieneCard.tsx';
import { DefaultProductCard } from './Cards/DefaultProductCard/DefaultProductCard.tsx';
import styled from 'styled-components';
import { Pagination, Stack, Box } from '@mui/material';

type Product =
  | components['schemas']['GetProductResponse']
  | components['schemas']['GetPetFoodResponse']
  | components['schemas']['GetGroomingAndHygieneResponse'];

export const ProductList = () => {
  const [page, setPage] = useState(1);
  const ITEMS_PER_PAGE = 12;

  const { data, isLoading } = $api.useQuery('get', '/products');

  if (isLoading) {
    return <div>Loading...</div>;
  }

  const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const renderProduct = (product: Product) => {
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

  const totalProducts = data?.totalCount || 0;
  const totalPages = Math.ceil(totalProducts / ITEMS_PER_PAGE);

  const currentProducts: Array<Product> =
    data?.results?.slice((page - 1) * ITEMS_PER_PAGE, page * ITEMS_PER_PAGE) || [];

  return (
    <>
      <ProductListContainer>
        {currentProducts.map(product => (
          <div key={product.id} className="product-item">
            {renderProduct(product)}
          </div>
        ))}
      </ProductListContainer>

      {totalPages > 1 && (
        <PaginationContainer>
          <Stack spacing={2}>
            <Pagination
              count={totalPages}
              page={page}
              onChange={handlePageChange}
              color="primary"
              size="large"
              showFirstButton
              showLastButton
            />
          </Stack>
        </PaginationContainer>
      )}
    </>
  );
};

const ProductListContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
  padding: 1.25rem;
`;

const PaginationContainer = styled(Box)`
  display: flex;
  justify-content: center;
  padding: 2rem 0;
`;
