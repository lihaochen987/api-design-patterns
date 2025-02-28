import styled from 'styled-components';

export const ProductListContainer = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.25rem;
  padding: 1.25rem;
`;

export const Price = styled.span`
  font-weight: bold;
  font-size: 1.2rem;
  color: #e53935;
`;
