import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logo from '../images/logo.png';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header className="border-bottom-1 d-flex">
                <Navbar className="flex-grow-1 navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3 bg-dark navbar-dark" light>
                    <Container>
                        <div className="">
                            <img src={logo} width="80" height="80" className="d-inline-block align-top p-2 position-absolute bg-dark border" alt=""
                                style={{ borderRadius: '50px', top: '7px'} } />
                        </div>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse text-light flex-grow-0" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow mt-5 mt-sm-1 mt-lg-1 mt-md-1">
                                <NavItem>
                                    <NavLink tag={Link} className=" text-light" to="/" onClick={() => {
                                    }}>Home</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className=" text-light" to="/verify" onClick={() => {
                                    }}>Activation</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className=" text-light" to="/terms" onClick={() => {
                                    }}>Terms</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className=" text-light" to="/verifytutorial" onClick={() => {
                                    }}>Activation Guide!</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
