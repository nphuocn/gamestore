import { useEffect, useState } from "react";
import type { GameDetail } from "../models/gamesdetail.model";

function GameListComponent() {  

  const [games, setGames] = useState<GameDetail[]>();

  useEffect(() => {
    const fetchGames = async () => {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/game`);
      const data = await response.json();
      setGames(data);
    };

    fetchGames();
  }, []);

  return (
    <>
      <div className="container">
        <h1>List of Games</h1>
        <button className="btn btn-primary mb-3">Add New Game</button>
        {games && games.length === 0 && (
          <div className="alert alert-warning" role="alert">
            No games available.
          </div>
        )}
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Id</th>
              <th>Title</th>
              <th>Genre</th>
              <th>Price</th>
              <th>Release Date</th>
              <th>Developer</th>
              <th>Publisher</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {games &&
              games.map((game) => (
                <tr key={game.id}>
                  <th scope="row">{game.id}</th>
                  <td>{game.title}</td>
                  <td>{game.genre}</td>
                  <td>{game.price}</td>
                  <td>{game.releaseDate.toString().split("T")[0]}</td>
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
