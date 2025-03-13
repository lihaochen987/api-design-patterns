import React, { useState, useEffect } from 'react';
import { components } from '../../../Shared/types';
import { $api } from '../../../Shared/fetch-client.ts';
import { PetFoodCard } from './Cards/PetFoodCard/PetFoodCard.tsx';
import { GroomingAndHygieneCard } from './Cards/GroomingAndHygieneCard/GroomingAndHygieneCard.tsx';
import { DefaultProductCard } from './Cards/DefaultProductCard/DefaultProductCard.tsx';
import styled from 'styled-components';
import { Pagination, Stack, Box, CircularProgress } from '@mui/material';

type Product =
  | components['schemas']['GetProductResponse']
  | components['schemas']['GetPetFoodResponse']
  | components['schemas']['GetGroomingAndHygieneResponse'];

export const ProductList = () => {
  const [page, setPage] = useState(1);
  const ITEMS_PER_PAGE = 12;

  const { data, isLoading, refetch } = $api.useQuery('get', '/products', {
    params: {
      query: {
        PageToken: String(page),
        MaxPageSize: ITEMS_PER_PAGE,
      },
    },
  });

  useEffect(() => {
    void refetch();
  }, [page, refetch]);

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
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

  if (isLoading) {
    return (
      <LoadingContainer>
        <CircularProgress />
      </LoadingContainer>
    );
  }

  const totalProducts = data?.totalCount || 0;
  const totalPages = Math.ceil(totalProducts / ITEMS_PER_PAGE);

  const products: Array<Product> = data?.results || [];

  return (
    <>
      <ProductListContainer>
        {products.length === 0 ? (
          <NoProductsMessage>No products found</NoProductsMessage>
        ) : (
          products.map(product => (
            <div key={product.id} className="product-item">
              {renderProduct(product)}
            </div>
          ))
        )}
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

const LoadingContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 300px;
  width: 100%;
`;

const NoProductsMessage = styled.div`
  grid-column: 1 / -1;
  text-align: center;
  padding: 2rem;
  font-size: 1.2rem;
  color: #666;
`;
