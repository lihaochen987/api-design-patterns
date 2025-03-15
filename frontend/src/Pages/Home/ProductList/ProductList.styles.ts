import styled from 'styled-components';

export const Price = styled.span`
  font-weight: bold;
  font-size: 1.2rem;
  color: #e53935;
`;

export const ProductDimensions = styled.div`
  margin-bottom: 0.5rem;
`;

export const ProductSpecs = styled.div`
  margin-bottom: 0.5rem;
`;

export const SectionTitle = styled.h4`
  font-size: 1rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: #333;
`;

export const Card = styled.div`
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  background-color: #fff;
`;

export const CardHeader = styled.div`
  padding: 1rem;
  border-bottom: 1px solid #f0f0f0;
`;

export const CardContent = styled.div`
  padding: 1rem;
`;

export const CardActions = styled.div`
  display: flex;
  padding: 0.5rem 1rem;
  justify-content: space-evenly;
`;

export const CardButton = styled.button<{ variant?: 'contained' | 'outlined' }>`
  padding: 0.5rem 1rem;
  border-radius: 4px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s;

  ${props =>
    props.variant === 'contained' &&
    `
    background-color: #1976d2;
    color: white;
    border: none;
    &:hover {
      background-color: #1565c0;
    }
  `}

  ${props =>
    props.variant === 'outlined' &&
    `
    background-color: transparent;
    color: #1976d2;
    border: 1px solid #1976d2;
    &:hover {
      background-color: rgba(25, 118, 210, 0.04);
    }
  `}
`;
