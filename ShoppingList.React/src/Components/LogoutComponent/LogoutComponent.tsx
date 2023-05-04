import React from "react";
import axios from "axios";
import { NavComponent, NavComponentProps, navHOC } from "../NavComponent/NavComponen";
import { GetUserEndpoint, LogoutEndpoint } from "../../Endpoints";
import { Button, Row } from "react-bootstrap";
import UserDTO from "../../DTO/UserDTO";

interface ICompState {
  username: string | null
}

class LogoutComponent extends NavComponent<{}, ICompState> {
  constructor(props: NavComponentProps<{}>) {
    super(props);

    this.state = {
      username: null
    }
  }

  componentDidMount(): void {
    this.fetchUserName();
  }

  render() {
    return <Row>
      <span>Hi <b>{this.state.username ?? "UNKNOWN"}</b>!<Button variant="link" className="p-1" onClick={this.logout}>Logout</Button></span>
    </Row>;
  }

  fetchUserName = async () => {
    try {
      const resp = await axios.get<UserDTO>(GetUserEndpoint);
      this.setState({ username: resp.data.userName })
    } catch (err) {
      console.error(err);
    }
  }

  logout = async () => {
    try {
      await axios.post(LogoutEndpoint);
      this.props.navigate('/SignIn');
    } catch (err) {
      console.log(err);
    }

  }
}

export default navHOC(LogoutComponent);