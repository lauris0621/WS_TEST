class CoordCalculator extends React.Component {
    constructor(props) {
        super(props);
        this.state = { maxCoordinates: '', data: [] };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    componentWillMount() {
        this.addRover(1);
    }

    addRover(id) {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + '/' + id, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.state.data.push(data);
            this.setState({ data: this.state.data });
        };
        xhr.send();
    }

    handleInputChange(e) {
        const target = e.target;
        const value = target.value;
        const name = target.name;
        if (name === 'maxCoordinates') {
            this.setState({ maxCoordinates: value });
        } else {
            const currentRover = this.state.data[this.state.data.length - 1];
            currentRover[name] = value;
            this.setState({ data: this.state.data });
        }
    }

    handleSubmit(e) {
        e.preventDefault();
        const currentRover = this.state.data[this.state.data.length - 1];
        const data = new FormData();
        data.append('id', currentRover.id);
        data.append('maxCoordinates', this.state.maxCoordinates);
        data.append('currentPosition', currentRover.currentPosition);
        data.append('movement', currentRover.movement);
        data.append('result', currentRover.result);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.state.data[this.state.data.length - 1] = data;
            this.addRover(this.state.data.length + 1);
            this.setState({ data: this.state.data });
        }
        xhr.send(data);
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="mb-3">
                        <div className="col-md-6">
                            <label htmlFor="maxCoordinates">Max Coordinates</label>
                            <input type="text" name="maxCoordinates" id="maxCoordinates" placeholder="Max Coordinates ex: 5 5" className="form-control" onChange={this.handleInputChange} />
                        </div>
                    </div>
                    <React.Fragment>
                        <CoordCalculatorRoverList data={this.state.data} onChange={this.handleInputChange} />
                    </React.Fragment>
                    <div className="mb-3">
                        <div className="col-md-6">
                            <input type="submit" value="Calculate" className="btn btn-primary" />
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}

class CoordCalculatorRoverList extends React.Component {
    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(e) {
        this.props.onChange(e);
    }

    render() {
        const roverList = this.props.data.map(rover => (
            <CoordCalculatorRover
                key={rover.id}
                id={rover.id}
                currentPosition={rover.currentPosition}
                movement={rover.movement}
                result={rover.result}
                onChange={this.handleInputChange}
            />
        ));
        return (
            <div>
                {roverList}
            </div>
        );
    }
}

class CoordCalculatorRover extends React.Component {
    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(e) {
        this.props.onChange(e);
    }

    render() {
        return (
            <div>
                <div className="mb-3">
                    <div className="col-md-6">
                        <h4>Rover {this.props.id}</h4>
                    </div>
                </div>
                <div className="mb-3">
                    <div className="col-md-6">
                        <label htmlFor="currentPosition">Current Position</label>
                        <input name="currentPosition" type="text" id="currentPosition" placeholder="Current Position ex: 1 2 N" className="form-control" value={this.props.currentPosition} onChange={this.handleInputChange} />
                    </div>
                </div>
                <div className="mb-3">
                    <div className="col-md-6">
                        <label htmlFor="movement">Movement</label>
                        <input type="text" name="movement" id="movement" placeholder="Movement ex: LMRML" className="form-control" value={this.props.movement} onChange={this.handleInputChange} />
                    </div>
                </div>
                <React.Fragment>
                    <CoordCalculatorResult result={this.props.result} />
                </React.Fragment>
            </div>
        );
    }
}

class CoordCalculatorResult extends React.Component {
    render() {
        return (
            <div className="mb-3">
                <div className="col-md-3">
                    <label>Result</label>
                </div>
                <div className="col-md-3">
                    {this.props.result}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<CoordCalculator url={addRoverUrl} submitUrl={submitRoverUrl} />, document.getElementById('content'));
