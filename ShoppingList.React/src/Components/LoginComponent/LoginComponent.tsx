import React from "react";
import { Button, Col, Form, FormControl, FormFloating, Row } from "react-bootstrap";

class LoginComponent<T> extends React.Component<T> {
  constructor(props: T) {
    super(props);

    this.state = {
      username: String,
      password: String
    }
  }

  render() {
    return (
      <Row className="justify-content-center">
        <Col md="6">
          <h2 className="display-2 text-center my-5">Shopping List</h2>
          <Form className="mb-4">
            <h2>Sign in</h2>
            <FormFloating className="mb-3">
              <FormControl type="text" id="floatingInput"
                placeholder="Username" required={true} onChange={(event) => {
                  this.setState({ username: event.target.value});
                }}/>
              <label htmlFor="floatingInput">Username</label>
            </FormFloating>
            <FormFloating className="mb-3">
              <FormControl type="password" id="floatingPassword"
                placeholder="Password" required={true} onChange={(event) => {
                  this.setState({ password: event.target.value});
                }}/>
              <label htmlFor="floatingPassword">Password</label>
            </FormFloating>

            <Row>
              <Col className="d-grid">
                <Button variant="primary" className="p-2" type="submit">Sign in</Button>
              </Col>
            </Row>
          </Form>
          <span>Don't have an account? <a href="https://google.com">Sign up</a></span>
        </Col>
      </Row>
    );
  }

  signIn() {
    
  }
}

export default LoginComponent;