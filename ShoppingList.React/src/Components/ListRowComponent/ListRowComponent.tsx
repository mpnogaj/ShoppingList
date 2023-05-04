import React from "react"
import ListDTO from "../../DTO/ListDTO"
import { Button } from "react-bootstrap";
interface ICompProps {
  list: ListDTO,
  onDeleteList: (list: ListDTO) => void
}

class ListRowComponent extends React.Component<ICompProps> {
  render() {
    return (
    <li>
      {this.props.list.name}
      <Button variant="link">View</Button>
      <Button variant="link" onClick={() => this.props.onDeleteList(this.props.list)}>Delete</Button>
    </li>)
  }
}

export default ListRowComponent