import axios from "axios";
import { useState } from "react";
import './styles.css';



const EditPedido = (pedidoItem, fetchPedidos) => {

    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [pedidoEdit, setPedidoEdit] = useState(null);
    const [error, setError] = useState("");

    
    const openEditModal = (pedido) => {
        setPedidoEdit(pedido);
        setIsEditModalOpen(true);
    };

    const closeEditModal = () => {
        setIsEditModalOpen(false);
        setPedidoEdit(null);
    };

    const handleEditChange = (field, value) => {
        setPedidoEdit({ ...pedidoEdit, [field]: value });
    };

    const handleEditItemChange = (index, field, value) => {
        const newItens = [...pedidoEdit.itens];
        newItens[index][field] = value;
        setPedidoEdit({ ...pedidoEdit, itens: newItens });
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.put(`/api/pedidos/${pedidoEdit.id}`, pedidoEdit, {
                headers: {
                    "Content-Type": "application/json"
                }
            });
            alert('Pedido atualizado com sucesso:', response.data);
            closeEditModal();
            fetchPedidos();
        } catch (error) {
            console.error('Erro ao atualizar pedido:', error);
            setError(error?.response.data)
        }
    }
    console.log(pedidoItem)
    console.log(pedidoEdit)
    return(
        <div>
            <div>
                <button onClick={() => openEditModal(pedidoItem.pedido)}>Editar</button>
            </div>
            {
                isEditModalOpen && pedidoEdit && (
                <div className="modal">
                    <div className="modal-content">
                        <span className="close" onClick={closeEditModal}>&times;</span>
                        <h2>Editar Pedido</h2>
                        <form onSubmit={handleEditSubmit}>
                            <label>
                                Cliente:
                                <input type="text" value={pedidoEdit?.cliente.nome} readOnly />
                            </label>
                            <label>
                                Data do Pedido:
                                <input 
                                    type="datetime-local" 
                                    value={pedidoEdit?.dataPedido ? new Date(pedidoEdit.dataPedido).toISOString().slice(0, 16) : ''} 
                                    onChange={(e) => handleEditChange('dataPedido', e.target.value)} required 
                                />
                            </label>
                            <h3>Itens</h3>
                            {pedidoEdit?.itens.map((item, index) => (
                                <div key={item.id} className="item">
                                    <label className='produto'>
                                        Produto:
                                        <input type="text" value={item.produto} onChange={(e) => handleEditItemChange(index, 'produto', e.target.value)} required />
                                    </label>
                                    <div className='quantidade-preco'>
                                        <label>
                                            Quantidade:
                                            <input type="number" value={item.quantidade} onChange={(e) => handleEditItemChange(index, 'quantidade', e.target.value)} required />
                                        </label>
                                        <label>
                                            Preço Unitário:
                                            <input type="number" step="0.01" value={item.precoUnitario} onChange={(e) => handleEditItemChange(index, 'precoUnitario', e.target.value)} required />
                                        </label>
                                    </div>
                                </div>
                            ))}
                            {error && <p className='error'>{error}</p>}
                            <button type="submit">Salvar</button>
                        </form>
                    </div>
                </div>
            )}
        </div>
    )
}

export default EditPedido