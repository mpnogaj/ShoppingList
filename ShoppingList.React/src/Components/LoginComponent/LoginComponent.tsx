import React from "react";
import { Button, Col, Form, FormControl, FormFloating, Row } from "react-bootstrap";
import axios, { AxiosError } from 'axios';
import { AuthenticateEndpoint } from "../../Endpoints";

interface ICompState {
  username: string,
  password: string,
  error: string,
  singInBtnEnabled: boolean
}

class LoginComponent<T> extends React.Component<T, ICompState> {
  constructor(props: T) {
    super(props);

    this.state = {
      username: "",
      password: "",
      error: "",
      singInBtnEnabled: true
    };
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
                  this.setState({ username: event.target.value });
                }} value={this.state.username} />
              <label htmlFor="floatingInput">Username</label>
            </FormFloating>
            <FormFloating className="mb-3">
              <FormControl type="password" id="floatingPassword"
                placeholder="Password" required={true} onChange={(event) => {
                  this.setState({ password: event.target.value });
                }} value={this.state.password} />
              <label htmlFor="floatingPassword">Password</label>
            </FormFloating>

            <Row>
              <span className="error-text mb-2" style={{color: "red"}}>{this.state.error}</span>
            </Row>

            <Row>
              <Col className="d-grid">
                <Button variant="primary" className="p-2" type="submit" onClick={async (e) => {
                  e.preventDefault();
                  await this.signIn();
                }} disabled={!this.state.singInBtnEnabled}>Sign in</Button>
              </Col>
            </Row>
          </Form>
          <span>Don't have an account? <a href="https://google.com">Sign up</a></span>
        </Col>
      </Row>
    );
  }

  signIn = async () => {
    this.setState({singInBtnEnabled: false});

    const payload: ILoginPayload = {
      username: this.state.username,
      password: this.state.password
    };

    try {
      const resp = await axios.get<string>(AuthenticateEndpoint, { params: payload });
      window.alert(resp.data);
    } catch (ex) {
      if (ex instanceof AxiosError) {
        console.log(ex);
        if (ex.response === undefined || ex.response?.status === undefined) {
          this.setState({ error: ex.message });
        } else {
          this.setState({ error: this.getErrorMessage(ex.response.status) ?? ex.message });
        }
      }
    }
    this.setState({singInBtnEnabled: true});
  }

  getErrorMessage = (errorCode: number): string | undefined => {
    switch (errorCode) {
      case 500:
        return 'Internal server error.';
      case 401:
        return 'Invalid username or password.';
      case 404:
        return 'Not found.';
      default:
        return undefined;
    }
  }
}

interface ILoginPayload {
  username: string,
  password: string
}

export default LoginComponent;