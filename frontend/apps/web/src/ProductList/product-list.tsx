import React from "react";
import {$api} from "@repo/product-data-access/api";
import {components} from "@repo/api-types";

export const ProductList = () => {
    const {data, isLoading} = $api.useQuery("get", "/products");

    if (isLoading) {
        return <div>Loading...</div>;
    }

    const renderProduct = (product:
                           (components["schemas"]["GetProductResponse"]
                               | components["schemas"]["GetPetFoodResponse"]
                               | components["schemas"]["GetGroomingAndHygieneResponse"])) => {
        switch (product.category) {
            case "PetFood":
                return (<div>PetFood!</div>)
            case "GroomingAndHygiene":
                return (<div>GroomingAndHygiene!</div>)
            default:
                return (<div>Base product</div>)
        }
    }

    return (
        <div>
            {data?.results?.map((product) => renderProduct(product))}
        </div>
    );
};
