import {components} from "../../../shared/types";
import {
    AddToCartButton,
    Price,
    ProductActions,
    ProductCard,
    ProductHeader,
    ViewDetailsButton
} from "../ProductList.styles.ts";

interface GroomingCardProps {
    product: components["schemas"]["GetGroomingAndHygieneResponse"];
}

export const GroomingCard = ({product}: GroomingCardProps) => {
    return (
        <ProductCard variant="grooming">
            <ProductHeader>
                <h3>{product.name}</h3>
                <Price>${product.price}</Price>
            </ProductHeader>

            <div>
                <p>Grooming and Hygiene Product</p>
                {/* Additional GroomingAndHygiene specific fields can be added here */}
            </div>

            <ProductActions>
                <AddToCartButton>Add to Cart</AddToCartButton>
                <ViewDetailsButton>View Details</ViewDetailsButton>
            </ProductActions>
        </ProductCard>
    );
};