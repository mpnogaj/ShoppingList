import React from 'react';
import './App.css';
import LoginComponent from './Components/LoginComponent/LoginComponent'
import { Container } from 'react-bootstrap';

function App() {
  return (
      <Container fluid>
        <LoginComponent/>
      </Container>
  );
}

export default App;
