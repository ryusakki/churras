import api from '../api';

class UsuarioService {
    async autenticar(credenciais) {
        const userSession = {
            auth: String.empty,
            ...credenciais
        }

        const request = await api.post('/Autenticar', credenciais);
        if (request?.status === 200) {
            userSession.auth = `Bearer ${request?.data?.token}`;
            localStorage.setItem('userSession', JSON.stringify(userSession));
        }
        return request?.status;
    }

    async registrar(usuario) {
        const request = await api.post('/RegistrarUsuario', usuario);
        return request?.status;
    }
}

const usuarioService = new UsuarioService();
export default usuarioService;
