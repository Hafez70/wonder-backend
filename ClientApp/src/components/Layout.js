import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div className="d-flex flex-column flex-grow-1 col p-0 m-0">
                <NavMenu />
                <Container className="bg-dark pt-5 overflow-hidden d-flex flex-column flex-grow-1">
                    {this.props.children}
                </Container>
                <footer className="border-top footer-sticky p-0 m-0 bg-dark ">
                    <p className="text-center p-2">all rights reserved to the WonderTools developer team</p></footer>
            </div>
        );
    }
}
