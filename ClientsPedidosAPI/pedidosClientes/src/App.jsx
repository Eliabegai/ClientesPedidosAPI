import './App.css'
import PedidoList from './components/PedidoList'

function App() {

  return (
    <>
      <div>
        <h1>Pedido dos Clientes</h1>
      </div>
      <div className="card">
        <PedidoList />
      </div>
    </>
  )
}

export default App
