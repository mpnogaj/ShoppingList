import React from "react";
import axios from "axios";
import { Navigate } from "react-router";

interface ICompState {
  loading: boolean,
  authorized: boolean;
}

interface ICompProps {
  child: JSX.Element
}

class ProtectedRoute extends React.Component<ICompProps, ICompState> {
  constructor(props: ICompProps) {
    super(props);

    this.state = {
      loading: true,
      authorized: false
    };
  }

  componentDidMount(): void {
    this.authenticate();
  }

  render(): React.ReactNode {
    if (this.state.loading) return <p>Loading</p>
    return this.state.authorized ? this.props.child : <Navigate to="/SignIn" />
  }

  authenticate = async (): Promise<void> => {
    try {
      const res = await axios.get("/api/Ping/AuthPing");
      if (res.status === 200) {
        this.setState({ loading: false, authorized: true });
      } else {
        this.setState({ loading: false, authorized: false });
      }
    } catch {
      this.setState({ loading: false, authorized: false });
    }
  };
}

export default ProtectedRoute;