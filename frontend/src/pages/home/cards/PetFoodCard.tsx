import {components} from "../../../shared/types";
import {
    AddToCartButton,
    Price,
    ProductActions,
    ProductCard,
    ProductHeader,
    ViewDetailsButton
} from "../ProductList.styles.ts";
import {
    IngredientsSection,
    NutritionKey,
    NutritionRow,
    NutritionSection,
    NutritionTable, NutritionValue,
    ProductDetails,
    ProductDimensions,
    ProductSpecs, SectionTitle, StorageSection
} from "./PetFoodCard.styles.ts";

interface PetFoodCardProps {
    product: components["schemas"]["GetPetFoodResponse"];
}

export const PetFoodCard = ({product}: PetFoodCardProps) => {
    return (
        <ProductCard variant="petFood">
            <ProductHeader>
                <h3>{product.name}</h3>
                <Price>${product.price}</Price>
            </ProductHeader>

            <ProductDetails>
                <ProductSpecs>
                    <p><strong>Age Group:</strong> {product.ageGroup}</p>
                    <p><strong>Breed Size:</strong> {product.breedSize}</p>
                    <p><strong>Weight:</strong> {product.weightKg} kg</p>
                </ProductSpecs>

                <ProductDimensions>
                    <p>
                        <strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x {product.dimensions.height}
                    </p>
                </ProductDimensions>

                <NutritionSection>
                    <SectionTitle>Nutritional Information</SectionTitle>
                    <NutritionTable>
                        {Object.entries(product.nutritionalInfo).map(([key, value]) => (
                            <NutritionRow key={key}>
                                <NutritionKey>{key}:</NutritionKey>
                                <NutritionValue>{value}</NutritionValue>
                            </NutritionRow>
                        ))}
                    </NutritionTable>
                </NutritionSection>

                <IngredientsSection>
                    <SectionTitle>Ingredients</SectionTitle>
                    <p>{product.ingredients}</p>
                </IngredientsSection>

                <StorageSection>
                    <SectionTitle>Storage Instructions</SectionTitle>
                    <p>{product.storageInstructions}</p>
                </StorageSection>
            </ProductDetails>

            <ProductActions>
                <AddToCartButton>Add to Cart</AddToCartButton>
                <ViewDetailsButton>View Details</ViewDetailsButton>
            </ProductActions>
        </ProductCard>
    );
};