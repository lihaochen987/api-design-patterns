import {components} from "../../../shared/types";
import {
    Price,
} from "../ProductList.styles.ts";
import {
    IngredientsSection,
    NutritionKey,
    NutritionRow,
    NutritionSection,
    NutritionTable, NutritionValue,
    ProductDimensions,
    ProductSpecs, SectionTitle, StorageSection
} from "./PetFoodCard.styles.ts";
import {Button, Card, CardActions, CardContent, CardHeader} from "@mui/material";

interface PetFoodCardProps {
    product: components["schemas"]["GetPetFoodResponse"];
}

export const PetFoodCard = ({product}: PetFoodCardProps) => {
    return (
        <Card>
            <CardHeader
                title={<h3>{product.name}</h3>}
                subheader={<Price>${product.price}</Price>}
            />

            <CardContent>
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
            </CardContent>

            <CardActions>
                <Button variant="contained">Add to cart</Button>
                <Button variant="outlined">View Details</Button>
            </CardActions>
        </Card>
    );
};