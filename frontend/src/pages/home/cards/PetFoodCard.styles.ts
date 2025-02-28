import styled from "styled-components";

export const ProductDetails = styled.div`
    display: flex;
    flex-direction: column;
    gap: 12px;
`;

export const ProductSpecs = styled.div`
    margin-bottom: 8px;
`;

export const ProductDimensions = styled.div`
    margin-bottom: 8px;
`;

export const NutritionSection = styled.div`
    margin-top: 12px;
`;

export const NutritionTable = styled.div`
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 8px;
    margin-top: 8px;
`;

export const NutritionRow = styled.div`
    display: flex;
    justify-content: space-between;
    padding: 4px;
    background-color: #f9f9f9;
    border-radius: 4px;
`;

export const NutritionKey = styled.span`
    font-weight: 500;
`;

export const NutritionValue = styled.span``;

export const IngredientsSection = styled.div`
    margin-top: 12px;
`;

export const StorageSection = styled.div`
    margin-top: 12px;
`;

export const SectionTitle = styled.h4`
    font-size: 1rem;
    font-weight: 600;
    margin-bottom: 8px;
    color: #333;
`;