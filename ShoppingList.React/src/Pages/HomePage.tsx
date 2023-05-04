import React from "react";
import LogoutComponent from "../Components/LogoutComponent/LogoutComponent";
import { Col } from "react-bootstrap";
import AppHeaderComponent from "../Components/AppHeaderComponent/AppHeaderComponent";

class HomePage extends React.Component {
  render() {
    return (
      <Col>
        <LogoutComponent />
        <AppHeaderComponent />


      </Col>
    );
  }
}

export default HomePage;