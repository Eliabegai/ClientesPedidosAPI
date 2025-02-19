import axios from "axios"
import { useState } from "react"
import "./styles.css"


const CadastrarNovoCliente = () => {

    const [isModalOpen, setIsModalOpen] = useState(false)
    const [nome, setNome] = useState('')
    const [email, setEmail] = useState('')
    const [dataNascimento, setDataNascimento] = useState('')
    const [cpf, setCpf] = useState('')
    const [endereco, setEndereco] = useState('')
    const [error, setError] = useState('')

    const cadastrar = () => {
        setIsModalOpen(true)
      }
    
      const closeModal = () => {
        setIsModalOpen(false)
      }
    
      const handleSubmit = async (e) => {
        e.preventDefault()
        const cliente = { nome, email, dataNascimento, cpf, endereco }
        try {
          const response = await axios.post('/api/clientes', cliente, {
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


    return(
        <div>
            <button onClick={cadastrar}>Cadastrar Novo Cliente</button>

            {isModalOpen && (
                <div className="modal">
                    <div className="modal-content">
                        <span className="close" onClick={closeModal}>&times;</span>
                        <h2>Cadastrar Cliente</h2>
                        <form onSubmit={handleSubmit}>
                            <label>
                                Nome:
                                <input type="text" value={nome} onChange={(e) => setNome(e.target.value)} required />
                            </label>
                            <label>
                                Email:
                                <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
                            </label>
                            <label>
                                Data de Nascimento:
                                <input type="date" value={dataNascimento} onChange={(e) => setDataNascimento(e.target.value)} required />
                            </label>
                            <label>
                                CPF:
                                <input type="text" value={cpf} onChange={(e) => setCpf(e.target.value)} required />
                            </label>
                            <label>
                                Endereço:
                                <input type="text" value={endereco} onChange={(e) => setEndereco(e.target.value)} required />
                            </label>
                            {error && <p className='error'>{error}</p>}
                            <button type="submit">Cadastrar</button>
                        </form>
                    </div>
                </div>
            )}
            
        </div>
    )
}

export default CadastrarNovoCliente