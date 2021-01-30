import api from '../api';

class AgendaService {
    headers() {
        const session = JSON.parse(localStorage.getItem('userSession'));
        return {
            headers: {
                Authorization: session.auth
            }
        };
    }

    async participar(evento) {
        const session = JSON.parse(localStorage.getItem('userSession'));
        const join = {
            evento: evento.data,
            contribuicao: parseFloat(evento.contribuicao),
            usuarioEmail: session.email
        }

        const headers = this.headers();
        const request = await api.post('/Agenda/ConfirmarParticipacao', join, headers);
        return request?.status == 200 ? request?.data : undefined;
    }

    async cancelarParticipacao(data) {
        const session = JSON.parse(localStorage.getItem('userSession'));
        const headers = this.headers();
        headers.data = {
            evento: data,
            usuarioEmail: session.email
        }
        const request = await api.delete('/Agenda/CancelarParticipacao', headers);
        return request?.data;
    }

    async listarChurrascos() {
        const headers = this.headers();
        const request = await api.get('/Agenda/ListarChurrascosAgendados', headers);
        return request?.data;
    }

    async listarParticipantes(dataChurrasco) {
        const headers = this.headers();
        const request = await api.post('/Agenda/ListarParticipantes', dataChurrasco, headers);
        return request?.data;
    }

    async agendarChurrasco(churrasco) {
        const headers = this.headers();
        const request = await api.post('/Agenda/AgendarChurrasco', churrasco, headers);
        return request.status;
    }
}

const agendaService = new AgendaService();
export default agendaService;