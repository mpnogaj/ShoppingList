import React from "react";

import SignInUpComponent from "../Components/SignInUpComponent/SignInUpComponent";

class SignUpPage extends React.Component {
  render() {
    return <SignInUpComponent isSignIn={false} />
  }
}

export default SignUpPage