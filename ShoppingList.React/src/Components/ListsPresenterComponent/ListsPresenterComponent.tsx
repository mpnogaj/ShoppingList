import axios from "axios";
import ListDTO from "../../DTO/ListDTO";
import { GetListsEndpoint } from "../../Endpoints";
import React from "react";
import ListRowComponent from "../ListRowComponent/ListRowComponent";
import { nanoid } from "nanoid";
import { Button, Col } from "react-bootstrap";

interface ICompState {
  lists: Array<ListDTO> | null,
  error: boolean
}

class ListsPresenterComponent extends React.Component<{}, ICompState> {
  constructor(props: {}) {
    super(props);

    this.state = {
      lists: null,
      error: false
    };
  }

  componentDidMount() {
    this.fetchLists();
  }

  render(): React.ReactNode {
    return (
      <Col>
        {this.renderLists()}
        <Button onClick={this.addNewList}>Add new list</Button>
      </Col>
    )
  }

  renderLists = () => {
    if (this.state.lists === null)
      return <span>Loading...</span>

    if (this.state.lists.length === 0)
      return <span>You don't have any lists. Try creating them!</span>

    return (
      <ol>
        {this.state.lists.map((val, ind) => {
          return <ListRowComponent key={nanoid()} list={val}
            onDeleteList={this.deleteList}  />
        })}
      </ol>
    )
  }

  fetchLists = async () => {
    try {
      const resp = await axios.get<Array<ListDTO>>(GetListsEndpoint);
      this.setState({ lists: resp.data, error: false });
    } catch (err) {
      this.setState({ lists: null, error: true });
      console.error(err);
    }
  }

  addNewList = async () => {
    try {
      const name = prompt('Enter list name:');

      if(name === null)
        return;

      if(name === '')
        throw Error('List name cannot be empty!');

      const payload = {
        listName: name
      };

      await axios.post(GetListsEndpoint, null, { params: payload });
      this.fetchLists();
    } catch (err) {
      if(err instanceof Error) {
        alert(err.message);
      }
      console.error(err);
    }
  }

  deleteList = async (list: ListDTO) => {
    try {
      console.assert(this.state.lists !== null);
      if (this.state.lists === null)
        throw Error('Critical error');

      const payload = {
        id: list.id
      };

      await axios.delete(GetListsEndpoint, { params: payload });
      const newLists = this.state.lists.filter(x => x.id !== list.id);
      this.setState({ lists: newLists });

    } catch (err) {
      if(err instanceof Error) {
        alert(err.message);
      }
      console.error(err);
    }
  }
}

export default ListsPresenterComponent;