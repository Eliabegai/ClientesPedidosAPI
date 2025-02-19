import './App.css'
import CadastrarNovoCliente from './components/CadastrarNovoCliente'
import PedidoList from './components/ListaPedido'

function App() {

  return (
    <>
      <div>
        <h1>Pedido dos Clientes</h1>
      </div>
      <div className='cadastro'>
        <CadastrarNovoCliente />
      </div>
      <div className="card">
        <PedidoList />
      </div>
    </>
  )
}

export default App
