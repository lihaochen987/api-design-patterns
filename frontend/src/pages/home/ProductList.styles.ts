import styled, {css} from 'styled-components';

type ProductCardVariant = 'petFood' | 'grooming' | 'default';

interface ProductCardProps {
    variant: ProductCardVariant;
}

export const ProductListContainer = styled.div`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 20px;
    padding: 20px;
`;

const getCardVariantStyles = (variant: ProductCardVariant) => {
    switch (variant) {
        case 'petFood':
            return css`
                border-left: 4px solid #4caf50;
            `;
        case 'grooming':
            return css`
                border-left: 4px solid #2196f3;
            `;
        default:
            return css`
                border-left: 4px solid #ff9800;
            `;
    }
};

export const Button = styled.button`
    padding: 8px 16px;
    border-radius: 4px;
    border: none;
    cursor: pointer;
    flex: 1;
    transition: background-color 0.2s ease;
`;


export const ProductCard = styled.div<ProductCardProps>`
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    padding: 16px;
    transition: box-shadow 0.3s ease;
    
    &:hover {
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }
    
    ${({ variant }) => getCardVariantStyles(variant)}
`;

export const ProductHeader = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 12px;
    border-bottom: 1px solid #f0f0f0;
    padding-bottom: 8px;
`;

export const Price = styled.span`
    font-weight: bold;
    font-size: 1.2em;
    color: #e53935;
`;

export const ProductActions = styled.div`
    display: flex;
    justify-content: space-between;
    margin-top: 16px;
    gap: 12px;
`;

export const AddToCartButton = styled(Button)`
    background-color: #4caf50;
    color: white;
    
    &:hover {
        background-color: #388e3c;
    }
`;

export const ViewDetailsButton = styled(Button)`
    background-color: #f5f5f5;
    color: #333;
    
    &:hover {
        background-color: #e0e0e0;
    }
`;