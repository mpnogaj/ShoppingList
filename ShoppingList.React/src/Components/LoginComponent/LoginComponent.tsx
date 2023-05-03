import React from "react";
import './LoginComponent.css'
class LoginComponent<T> extends React.Component<T> {
	constructor(props: T) {
		super(props);
	}
	
	render() {
		return (
			<div className="row vertical-center">
          <form className="col-xs-8 col-xs-offset-2  col-sm-6 col-sm-offset-3 col-md-4 col-sm-offset-4 col-lg-2 col-lg-offset-5">
            <h1>Sign In</h1>
            <p>
              <label className="sr-only" htmlFor="">Email Address</label>
              <input className="form-control" type="email" placeholder="Email Address" required></input>
            </p>
            <p>
              <label className="sr-only" htmlFor="">Password</label>
              <input className="form-control" type="email" placeholder="Password" required></input>
            </p>
            <p className="checkbox">
              <label><input type="checkbox"></input>Remember Me</label>
            </p>
            <button className="btn btn-primary btn-block" type="submit">Sign In</button>
          </form>
      </div>
		);
	}
}

export default LoginComponent;