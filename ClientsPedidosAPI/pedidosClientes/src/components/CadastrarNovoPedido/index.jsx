import { useEffect, useState } from 'react'
import './styles.css'
import axios from 'axios'

const CadastrarNovoPedido = () => {

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [clienteId, setClienteId] = useState('');
    const [dataPedido, setDataPedido] = useState('');
    const [itens, setItens] = useState([{ produto: '', quantidade: 1, precoUnitario: 0 }]);
    const [error, setError] = useState('');
    const [clientes, setClientes] = useState([]);

    const cadastrar = () => {
        setIsModalOpen(true)
    }
    
    const closeModal = () => {
        setIsModalOpen(false)
    }

    useEffect(() => {
        fetchClientes();
    }, [isModalOpen]);

    const fetchClientes = async () => {
        try {
            const response = await axios.get('/api/clientes/todos', {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            setClientes(response.data);
        } catch (error) {
            console.error('Erro ao buscar clientes:', error);
        }
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault()
        const pedido = { clienteId, dataPedido, itens };
        try {
            const response = await axios.post('/api/pedidos', pedido, {
            headers: {
                'Content-Type': 'application/json'
            }
            })
            alert('Cliente cadastrado com sucesso:', response.data)
            closeModal()
        } catch (error) {
            console.error('Erro ao cadastrar cliente:', error)
            setError(error?.response.data)
        }
    }

    const handleItemChange = (index, field, value) => {
        const newItens = [...itens];
        newItens[index][field] = value;
        setItens(newItens);
    };

    const addItem = () => {
        setItens([...itens, { produto: '', quantidade: 1, precoUnitario: 0 }]);
    };

    const removeItem = (index) => {
        const newItens = itens.filter((_, i) => i !== index);
        setItens(newItens);
    };

    return(
        <div>
            <button onClick={cadastrar}>Cadastrar Novo Pedido</button>

            {isModalOpen && (
                <div className="modal">
                    <div className="modal-content">
                        <span className="close" onClick={closeModal}>&times;</span>
                        <h2>Cadastrar Pedido</h2>
                        <form onSubmit={handleSubmit}>
                            <label>
                                Cliente ID: 
                                <select value={clienteId} onChange={e => setClienteId(e.target.value)} required>
                                    <option value="">Selecione um cliente</option>
                                    {clientes?.map((cliente) => (
                                        <option key={cliente.id} value={cliente.id}>
                                            {cliente.nome}
                                        </option>
                                    ))}
                                </select>
                            </label>
                            <label>
                                Data do Pedido:
                                <input type="datetime-local" value={dataPedido} onChange={(e) => setDataPedido(e.target.value)} required />
                            </label>
                            <h3>Itens</h3>
                            {itens.map((item, index) => (
                                <div key={index} className="item">
                                    <div className='produto'>
                                        <label>
                                            Produto:
                                            <input type="text" value={item.produto} onChange={(e) => handleItemChange(index, 'produto', e.target.value)} required />
                                        </label>
                                    </div>
                                    <div className='quantidade-preco'>
                                        <label>
                                            Quantidade:
                                            <input type="number" value={item.quantidade} onChange={(e) => handleItemChange(index, 'quantidade', e.target.value)} required />
                                        </label>
                                        <label>
                                            Preço Unitário:
                                            <input type="number" step="0.01" value={item.precoUnitario} onChange={(e) => handleItemChange(index, 'precoUnitario', e.target.value)} required />
                                        </label>
                                    </div>
                                    <div >
                                        <button className='remove-button' type="button" onClick={() => removeItem(index)}>Remover Item</button>
                                    </div>
                                </div>
                            ))}
                            {error && <p className='error'>{error}</p>}
                            <div className='buttons'>
                                <button type="button" onClick={addItem}>Adicionar Item</button>
                                <button type="submit">Cadastrar</button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    )
}

export default CadastrarNovoPedido