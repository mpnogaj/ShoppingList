import React from "react";
import { ParamsComponent, ParamsComponentProps, paramsHOC } from "../Components/HOC/ParamsComponent/ParamsComponent";
import { Empty } from "../Aliases";
import ErrorPage from "./ErrorPage";
import ListDTO from "../DTO/ListDTO";
import ListPresenterComponent from "../Components/ListPresenterComponent/ListPresenterComponent";
import axios from "axios";
import { GetListsEndpoint } from "../Endpoints";
import AppHeaderComponent from "../Components/AppHeaderComponent/AppHeaderComponent";
import { Col, Row } from "react-bootstrap";

type ICompParams = keyof {
  listId: number
};

interface ICompState {
  list: ListDTO | null,
  error: boolean
}

class ViewListPage extends ParamsComponent<ParamsComponentProps<Empty, ICompParams>, ICompParams, ICompState> {
  constructor(props: ParamsComponentProps<Empty, ICompParams>) {
    super(props);

    this.state = {
      list: null,
      error: false
    };
  }

  componentDidMount() {
    this.fetchList();
  }

  render() {
    if (this.state.error || this.props.params.listId === undefined || isNaN(Number(this.props.params.listId)))
      return <ErrorPage />

    if (this.state.list === null)
      return <p>Loading...</p>
    return (
      <Row className="justify-content-center">
        <Col md="6">
          <AppHeaderComponent />
          <ListPresenterComponent list={this.state.list} />
        </Col>
      </Row>
    );
  }

  fetchList = async () => {
    try {
      const resp = await axios.get<ListDTO>(`${GetListsEndpoint}/${this.props.params.listId}`);
      this.setState({ list: resp.data });
    } catch (err) {
      this.setState({ error: true, list: null });
      if (err instanceof Error)
        alert(err.message);
      console.log(err);
    }
  }
}

export default paramsHOC(ViewListPage);