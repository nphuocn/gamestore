function GameListComponent() {
  let games = [
    {
      id: 1,
      title: "The Legend of Zelda: Breath of the Wild",
      genre: "Action-adventure",
      price: 19.99,
      releaseDate: "2017-03-03",
      developer: "Nintendo",
      publisher: "Nintendo",
    },
    {
      id: 2,
      title: "The Witcher 3: Wild Hunt",
      genre: "RPG",
      price: 29.99,
      releaseDate: "2015-05-19",
      developer: "CD Projekt Red",
      publisher: "CD Projekt",
    },
    {
      id: 3,
      title: "Cyberpunk 2077",
      genre: "RPG",
      price: 9.99,
      releaseDate: "2020-12-10",
      developer: "CD Projekt Red",
      publisher: "CD Projekt",
    },
    {
      id: 4,
      title: "Red Dead Redemption 2",
      genre: "Action-Adventure",
      price: 39.99,
      releaseDate: "2018-10-26",
      developer: "Rockstar Games",
      publisher: "Rockstar Games",
    },
    {
      id: 5,
      title: "The Witcher 3: Wild Hunt",
      genre: "Sandbox",
      price: 29.99,
      releaseDate: "2011-11-18",
      developer: "Mojang Studios",
      publisher: "Mojang Studios",
    },
  ];

  //games = [];
  const headers = games.length ? Object.keys(games[0]) : [];

  const formatHeader = (h: string) =>
    h
      .replace(/([A-Z])/g, " $1")
      .replace(/[_-]/g, " ")
      .replace(/\b\w/g, (c) => c.toUpperCase());

  return (
    <>
      <div className="container">
        <h1>List of Games</h1>
        <button className="btn btn-primary mb-3">Add New Game</button>
        {games.length === 0 && (
          <div className="alert alert-warning" role="alert">
            No games available.
          </div>
        )}
        <table className="table table-striped">
          <thead>
            <tr>
              {headers.map((h) => (
                <th key={h}>{formatHeader(h)}</th>
              ))}
              {games.length > 0 && <th>Actions</th>}
            </tr>
          </thead>
          <tbody>
            {games.map((game) => (
              <tr key={game.id}>
                <th scope="row">{game.id}</th>
                <td>{game.title}</td>
                <td>{game.genre}</td>
                <td>{game.price}</td>
                <td>{game.releaseDate}</td>
                <td>{game.developer}</td>
                <td>{game.publisher}</td>
                <td>
                  <button className="btn btn-info">Edit</button>
                  <button className="btn btn-danger">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default GameListComponent;
