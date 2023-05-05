import React from "react";
import ListDTO from "../../DTO/ListDTO";
import { Button, Col, Row } from "react-bootstrap";
import { nanoid } from "nanoid";

interface ICompProps {
  list: ListDTO
}

interface ICompState {
  list: ListDTO
}

class ListPresenterComponent extends React.Component<ICompProps, ICompState> {
  constructor(props: ICompProps) {
    super(props);

    this.state = {
      list: this.props.list
    };
  }

  render() {
    return (
      <div>
        <Row className="justify-content-center text-center">
          <h1>List "{this.props.list.name}":</h1>
        </Row>

        {this.state.list.products.length === 0 ? <p className="text-center">Your list is empty. Try adding some new products</p> :
          <ul className="list-group">
            {this.state.list.products.map((val, ind) => {
              return (
                <li key={nanoid()} className="list-group-item">
                  {val}
                  <Button variant="link" onClick={() => {
                    const newProducts = this.state.list.products;
                    newProducts.splice(ind, 1);
                    console.log(newProducts);
                    this.setState({
                      list: {
                        ...this.state.list,
                        products: newProducts
                      }
                    });
                  }}>Delete</Button>
                </li>
              );
            })}
          </ul>}
        <Row className="mt-3">
          <Col className="d-flex p-1 justify-content-center">
            <Button className="mx-1" onClick={() => {
              const newProduct = prompt('New product:');
              if (newProduct === null || newProduct === '')
                return;
              const newProducts = this.state.list.products;
              newProducts.push(newProduct);
              this.setState({
                list: {
                  ...this.state.list,
                  products: newProducts
                }
              });
            }}>Add new product</Button>
            <Button className="mx-1">Save changes</Button>
          </Col>
        </Row>
      </div>
    );
  }
}

export default ListPresenterComponent;