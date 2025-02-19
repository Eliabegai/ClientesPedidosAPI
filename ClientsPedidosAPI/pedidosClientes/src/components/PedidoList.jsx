import { useEffect, useState } from "react"
import axios from "axios"
import "./styles.css"

const PedidoList = () => {
    const [pedidos, setPedidos] = useState([])
    // const [clienteNome, setClienteNome] = useState("")
    // const [inicio, setInicio] = useState("")
    // const [fim, setFim] = useState("")

    useEffect(() => {
        fetchPedidos()
    }, [])


    const fetchPedidos = async () => {
        try {
            const response = await axios.get("/api/pedidos",{
                headers: {
                    "Content-Type": "application/json"
                }
            })
            setPedidos(response.data)
        } catch (error) {
            console.error("Erro ao buscar pedido", error)
        }
    }

    console.log(pedidos)

    return(
        <div className="pedido-list-container">
            <h2>Lista de Pedidos</h2>

            <div className="pedidos">
                {
                    pedidos?.map((pedido) => (
                        <div key={pedido.id} className="pedido">
                            <h3 className="cliente-nome">Cliente: {pedido.cliente.nome}</h3>
                            <table className="itens-tabela">
                                <thead>
                                    <th>Produto</th>
                                    <th>Quantidade</th>
                                    <th>Preço Unitário</th>
                                    <th>Subtotal</th>
                                </thead>
                                <tbody>
                                    {
                                        pedido.itens.map((item) => (
                                            <tr key={item.id}>
                                                <td>{item.produto}</td>
                                                <td>{item.quantidade}</td>
                                                <td> {item.precoUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                                                <td> {item.subtotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                                            </tr>
                                        ))
                                    }
                                    
                                </tbody>
                            </table>
                            <div className="total">
                                <span>Data: {new Date(pedido.dataPedido).toLocaleDateString()}</span>
                                <span>Valor Total: {pedido.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                            </div>
                        </div>
                    ))
                }
            </div>
        </div>
    )
}

export default PedidoList