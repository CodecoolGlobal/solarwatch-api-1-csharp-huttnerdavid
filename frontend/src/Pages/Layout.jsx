import { Outlet, Link } from "react-router-dom";
const Layout = ({ isLoggedIn }) => {
  return (
    <div>
      <nav className="navBar">
        <ul>
          <li>
            <Link to="/">Home</Link>
          </li>
          {isLoggedIn ? (
            <li>
              <Link to="/solar-watch">SolarWatch</Link>
            </li>
          ) : (
            <>
              <li>
                <Link to="/registration">Register</Link>
              </li>
              <li>
                <Link to="/login">Login</Link>
              </li>
            </>
          )}
        </ul>
      </nav>

      <Outlet />
    </div>
  );
};
export default Layout;
