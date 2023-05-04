import ProductDTO from "./ProductDTO";

interface ListDTO {
    id: number,
    name: string,
    products: Array<ProductDTO>
}

export default ListDTO;