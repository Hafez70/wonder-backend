import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Home  from './components/Home';
import Verify from './components/Verify';
import Redirect from './components/Redirect';

import './custom.css'
import Terms from './components/Terms';
import ContactUs from './components/ContactUs';
import Tutorials from './components/Tutorials';
import VerifyTutorial from './components/VerifyTutorial';


export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/verify' component={Verify} />
                <Route path='/redirect' component={Redirect} />
                <Route path='/terms' component={Terms} />
                <Route path='/tutorials' component={Tutorials} />
                <Route path='/contactus' component={ContactUs} />
                <Route path='/verifytutorial' component={VerifyTutorial} />
            </Layout>
        );
    }
}
