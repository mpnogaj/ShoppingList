import React from "react";
import LogoutComponent from "../Components/LogoutComponent/LogoutComponent";
import { Col } from "react-bootstrap";
import AppHeaderComponent from "../Components/AppHeaderComponent/AppHeaderComponent";
import ListsPresenterComponent from "../Components/ListsPresenterComponent/ListsPresenterComponent";

class HomePage extends React.Component {
  render() {
    return (
      <Col>
        <LogoutComponent />
        <AppHeaderComponent />
        <ListsPresenterComponent />

      </Col>
    );
  }
}

export default HomePage;