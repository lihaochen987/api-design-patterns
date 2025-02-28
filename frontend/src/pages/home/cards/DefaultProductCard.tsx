import {components} from "../../../shared/types";
import {
    Price,
} from "../ProductList.styles.ts";
import {ProductDimensions} from "./DefaultProductCard.styles.ts";
import {Button, Card, CardActions, CardContent, CardHeader} from "@mui/material";

interface DefaultProductCardProps {
    product: components["schemas"]["GetProductResponse"];
}

export const DefaultProductCard = ({product}: DefaultProductCardProps) => {
    return (
        <Card>
            <CardHeader
                title={<h3>{product.name}</h3>}
                subheader={<Price>${product.price}</Price>}
            />

            <CardContent>
                <p><strong>Category:</strong> {product.category}</p>

                <ProductDimensions>
                    <p>
                        <strong>Dimensions:</strong> {product.dimensions.length} x {product.dimensions.width} x {product.dimensions.height}
                    </p>
                </ProductDimensions>
            </CardContent>

            <CardActions sx={{ justifyContent: 'space-evenly' }}>
                <Button variant="contained">Add to cart</Button>
                <Button variant="outlined">View Details</Button>
            </CardActions>
        </Card>
    );
};
