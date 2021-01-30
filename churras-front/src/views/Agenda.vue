<template>
  <div id="agenda">
    <header>
      <page-title
        style="flex-grow: 0.96; text-align: center"
        name="Churrascos agendados"
      />
      <b-button
        v-b-modal.modal-agenda-churras
        squared
        variant="outline-dark"
        id="tooltip-churras"
        style="font-size: 1.5rem"
      >
        <b-icon-plus />
      </b-button>
      <b-tooltip target="tooltip-churras" triggers="hover">
        Agendar churras
      </b-tooltip>
      <b-modal id="modal-agenda-churras" title="Agendar churrasco" hide-footer>
        <b-form @submit="onSubmit">
          <b-form-datepicker
            :date-disabled-fn="dateDisabled"
            v-model="date"
            class="mb-2"
            required
          />
          <b-form-group
            label="Descrição"
            label-for="input-desc"
            class="font-weight-bold"
          >
            <b-form-input
              id="input-desc"
              v-model="descricao"
              type="text"
              placeholder="Descrição"
              required
            ></b-form-input>
          </b-form-group>
          <b-form-group label="Observações adicionais" label-for="input-obs">
            <b-form-textarea
              id="input-obs"
              v-model="observacoesAdicionais"
              placeholder="Observações adicionais"
              rows="3"
              max-rows="6"
            ></b-form-textarea>
          </b-form-group>
          <b-button type="submit" block variant="dark">Agendar</b-button>
        </b-form>
      </b-modal>
    </header>
    <section>
      <b-modal
        size="lg"
        v-if="churrasco !== undefined"
        id="modal-churras-info"
        :title="churrasco.descricao"
        hide-footer
      >
        <p>Data: {{ `${churrasco.dia}/${churrasco.mes}/${churrasco.ano}` }}</p>
        <p />
        <p
          v-if="
            churrasco.observacoesAdicionais &&
            churrasco.observacoesAdicionais.length > 0
          "
        >
          Observações Adicionais: {{ churrasco.observacoesAdicionais }}
        </p>

        <b-table
          striped
          hover
          :fields="fields"
          :items="participantes"
        ></b-table>
        <b-form-input
          v-if="!hasJoinedAlready()"
          type="number"
          min="5"
          step="0.01"
          v-model="contribuicao"
          required
        ></b-form-input>
        <b-button
          variant="success"
          v-if="!hasJoinedAlready()"
          squared
          block
          class="mt-3"
          @click="onEventJoin"
          >Participar</b-button
        >
        <b-button
          variant="danger"
          v-if="hasJoinedAlready()"
          squared
          block
          class="mt-3"
          @click="onEventCancel"
          >Cancelar Participação</b-button
        >
      </b-modal>
      <b-card
        v-for="c in churrascos"
        v-bind:key="`${c.dia}/${c.mes}/${c.ano}`"
        :title="`${c.dia}/${c.mes}`"
        :sub-title="c.descricao"
        :ref="`${c.dia}/${c.mes}/${c.ano}`"
        style=""
      >
        <b-card-text>
          <b-icon-cash class="icon-color" />
          {{ c.valorArrecadado }}
        </b-card-text>
        <b-card-text>
          <b-icon-people-fill class="icon-color" />
          {{ c.totalParticipantes }}
        </b-card-text>

        <a
          href
          v-b-modal.modal-churras-info
          @click="onCardShow($event, c)"
          class="card-block stretched-link text-decoration-none"
        />
      </b-card>
    </section>
  </div>
</template>

<script>
import PageTitle from "../components/PageTitle";
import agendaService from "../services/AgendaService";

export default {
  name: "Agenda",

  components: {
    "page-title": PageTitle,
  },

  methods: {
    onSubmit(event) {
      event.preventDefault();
      this.$bvModal.hide("modal-agenda-churras");

      const date = new Date(this.date + "T00:00"); //ugh javascript
      const churrasco = {
        dia: date.getDate(),
        mes: date.getMonth() + 1,
        ano: date.getFullYear(),
        descricao: this.descricao,
        observacoesAdicionais: this.observacoesAdicionais,
      };

      agendaService.agendarChurrasco(churrasco).then(() => {
        this.$bvToast.toast(
          `Churrasco marcado para a data ${churrasco.dia}/${churrasco.mes}/${churrasco.ano}`,
          {
            title: "Churrasco agendado",
            variant: "success",
            solid: true,
          }
        );
      });
    },

    onCardShow(event, c) {
      event.preventDefault();
      agendaService
        .listarParticipantes({
          dia: c.dia,
          mes: c.mes,
          ano: c.ano,
        })
        .then((p) => {
          this.participantes = JSON.parse(JSON.stringify(p));
        });
      this.churrasco = JSON.parse(JSON.stringify(c));
    },

    onEventJoin(event) {
      event.preventDefault();

      if (!this.hasJoinedAlready()) {
        agendaService
          .participar({
            data: {
              dia: this.churrasco.dia,
              mes: this.churrasco.mes,
              ano: this.churrasco.ano,
            },
            contribuicao: this.contribuicao,
          })
          .then(() => {
            this.$bvToast.toast("Participação confirmada!", {
              title: "Ok",
              variant: "success",
              solid: true,
            });
          })
          .catch(() => {
            this.$bvToast.toast(
              "Não foi possível confirmar a participação no churrasco",
              {
                title: "Erro",
                variant: "danger",
                solid: true,
              }
            );
          });
      }
      this.$bvModal.hide("modal-churras-info");
    },

    onEventCancel() {
      agendaService
        .cancelarParticipacao({
          dia: this.churrasco.dia,
          mes: this.churrasco.mes,
          ano: this.churrasco.ano,
        })
        .then(() => {
          this.$bvToast.toast("Participação cancelada!", {
            title: "Ok",
            variant: "success",
            solid: true,
          });
        })
        .catch(() => {
          this.$bvToast.toast(
            "Não foi possível cancelar a participação no churrasco",
            {
              title: "Erro",
              variant: "danger",
              solid: true,
            }
          );
        });
      this.$bvModal.hide("modal-churras-info");
    },

    hasJoinedAlready() {
      return (
        this.participantes &&
        this.participantes.find((p) => p.email === this.session.email) !==
          undefined
      );
    },

    dateDisabled(ymd, date) {
      const currentDate = new Date();
      date.setHours(0, 0, 0, 0);
      currentDate.setHours(0, 0, 0, 0);
      const key = `${date.getDate()}/${
        date.getMonth() + 1
      }/${date.getFullYear()}`;
      return date < currentDate || this.$refs[key] !== undefined;
    },
  },

  data() {
    return {
      fields: [{ key: "nome" }, { key: "contribuicao", label: "Contribuição" }],
      session: JSON.parse(localStorage.getItem("userSession")),
      participantes: undefined,
      churrasco: undefined,
      contribuicao: 5,
      churrascos: [],
      date: String.empty,
      descricao: String.empty,
      observacoesAdicionais: String.empty,
    };
  },
  created() {
    agendaService
      .listarChurrascos()
      .then((r) => (this.churrascos = r))
      .catch((e) => {
        if (e.response.status === 401) {
          this.$router.push("/");
        }
      });
  },
};
</script>
<style scoped>
header {
  height: 20%;
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
}

section {
  height: 80%;
  background-color: #ffffffeb;
  padding: 2em;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
}

#agenda {
  height: 58.7em;
}

.icon-color {
  color: #fcd73c;
}

.card {
  border-radius: 0;
  width: 20rem; 
  max-height: 10.5rem;
  margin: 1.5em;

}

.card-title {
  font-weight: bold;
}

.card-body:hover {
  box-shadow: 0px 0px 8px rgba(0, 0, 0, 0.158);
}
</style>