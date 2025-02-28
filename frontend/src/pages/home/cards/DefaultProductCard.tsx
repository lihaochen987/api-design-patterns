import {components} from "../../../shared/types";
import {
    AddToCartButton,
    Price,
    ProductActions,
    ProductCard,
    ProductHeader,
    ViewDetailsButton
} from "../ProductList.styles.ts";
import {ProductDetails, ProductDimensions} from "./PetFoodCard.styles.ts";

interface DefaultProductCardProps {
    product: components["schemas"]["GetProductResponse"];
}

export const DefaultProductCard = ({product}: DefaultProductCardProps) => {
    return (
        <ProductCard variant="default">
            <ProductHeader>
                <h3>{product.name}</h3>
                <Price>${product.price}</Price>
            </ProductHeader>

            <ProductDetails>
                <p><strong>Category:</strong> {product.category}</p>

                <ProductDimensions>
                    <p>
                        <strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x {product.dimensions.height}
                    </p>
                </ProductDimensions>
            </ProductDetails>

            <ProductActions>
                <AddToCartButton>Add to Cart</AddToCartButton>
                <ViewDetailsButton>View Details</ViewDetailsButton>
            </ProductActions>
        </ProductCard>
    );
};
