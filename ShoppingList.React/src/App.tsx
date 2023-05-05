import React from 'react';
import './App.css';

import { Container } from 'react-bootstrap';
import { RouterProvider } from 'react-router';
import { createBrowserRouter } from 'react-router-dom';
import SignInPage from './Pages/SignInPage';
import SignUpPage from './Pages/SingUpPage';
import ProtectedRoute from './Components/ProtectedRoute/ProtectedRoute';
import ErrorPage from './Pages/ErrorPage';
import HomePage from './Pages/HomePage';
import ViewListPage from './Pages/ViewListPage';

function App() {

  const router = createBrowserRouter([
    {
      path: '/',
      element: <ProtectedRoute child={<HomePage />} />,
      errorElement: <ErrorPage />
    },
    {
      path: '/view/:listId',
      element: <ProtectedRoute child={<ViewListPage />} />
    },
    {
      path: '/SignIn',
      element: <SignInPage />
    },
    {
      path: '/SignUp',
      element: <SignUpPage />
    }
  ]);

  return (
    <Container fluid>
      <RouterProvider router={router} />
    </Container>
  );
}

export default App;
