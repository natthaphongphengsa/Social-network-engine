import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { posts: [], loading: true };
    }

    componentDidMount() {
        this.populateUsers();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderPosts(this.state.posts);
        return (
            <div className="row">
                <div className="col-12">
                    <h1 id="tabelLabel" >Wall of fame</h1>
                    <p>Fetching the greatest and latest from the social network </p>
                    {contents}
                </div>
            </div>
        );
    }

    static renderPosts(posts) {
        return (
            <React.Fragment>
                {posts.map(post =>
                    <div className='card m-3'>
                        <img className='card-img-top' />
                        <div className='card-body'>
                            <h5 className='card-title'>{post.message}</h5>
                            <p className='card-text'>{post.user.name}</p>
                            <p className="card-text">
                                <small className="text-muted">
                                    {post.created}
                                </small>
                            </p>
                        </div>
                    </div>
                )}
                <div className="clearfix"></div>
            </React.Fragment>
        );
    }

    async populateUsers() {
        const response = await fetch('wall/getusers');
        const data = await response.json();
        this.setState({ posts: data, loading: false });
    }
}
