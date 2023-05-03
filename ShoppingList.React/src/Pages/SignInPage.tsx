import React from "react";

import SignInUpComponent from "../Components/SignInUpComponent/SignInUpComponent";

class SignInPage extends React.Component {
  render() {
    return <SignInUpComponent isSignIn={true} />
  }
}

export default SignInPage