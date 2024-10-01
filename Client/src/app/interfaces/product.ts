import { Image } from "./image";
import { OrderItems } from "./order-items";

export interface Product
{
    id : number;
    name : string;
    price : number;
    category : string;
    stockQunatity : Number;
    orderItems : OrderItems[];
    images : Image[];
    description: string;
    rate: number;
}