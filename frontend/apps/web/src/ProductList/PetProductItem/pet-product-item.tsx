import React from "react"
import {PetProductItemProps} from "./pet-product-item.types.ts";

export const PetProductItem = ({product}: PetProductItemProps) => {
    return (<div>{product.name}</div>)
}